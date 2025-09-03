import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSelectModule } from '@angular/material/select';
import { ItemGroupsService } from 'src/app/services/item-groups.service';
import { ItemGroup } from 'src/interfaces/item-group';
import { FormControl, ReactiveFormsModule, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-item-group-select',
  standalone: true,
  imports: [CommonModule, MatSelectModule, ReactiveFormsModule],
  templateUrl: './item-group-select.component.html',
  styleUrl: './item-group-select.component.scss'
})
export class ItemGroupSelectComponent implements OnInit {
  @Input({ required: true }) selectedValue!: AbstractControl;
  @Input() defaultValue?: string; // 👈 optional default value (code)

  itemGroups: ItemGroup[] = [];

  selectedLevel1?: ItemGroup;
  selectedLevel2?: ItemGroup;
  selectedLevel3?: ItemGroup;

  constructor(private itemGroupService: ItemGroupsService) { }

  ngOnInit(): void {
    this.itemGroupService.getItemGroups().subscribe({
      next: data => {
        this.itemGroups = data;

        // 👇 if defaultValue exists, try to preselect
        if (this.defaultValue) {
          this.setDefaultSelection(this.defaultValue);
        }
      },
      error: err => console.error('Item Group API error', err)
    });
  }

  get level2Groups(): ItemGroup[] {
    return this.selectedLevel1?.childItemGroups ?? [];
  }

  get level3Groups(): ItemGroup[] {
    return this.selectedLevel2?.childItemGroups ?? [];
  }

  onLevel1Change(): void {
    this.selectedLevel2 = undefined;
    this.selectedLevel3 = undefined;
    this.updateSelectedCode();
  }

  onLevel2Change(): void {
    this.selectedLevel3 = undefined;
    this.updateSelectedCode();
  }

  onLevel3Change(): void {
    this.updateSelectedCode();
  }

  updateSelectedCode(): void {
    let selectedDesc = '';
    if (this.selectedLevel3?.code) {
      selectedDesc = `${this.selectedLevel1?.name} - ${this.selectedLevel2?.name} - ${this.selectedLevel3?.name}`;
    } else if (this.selectedLevel2?.code) {
      selectedDesc = `${this.selectedLevel1?.name} - ${this.selectedLevel2?.name}`;
    } else if (this.selectedLevel1?.code) {
      selectedDesc = this.selectedLevel1?.name;
    }

    const selectedCode =
      this.selectedLevel3?.code ??
      this.selectedLevel2?.code ??
      this.selectedLevel1?.code ??
      '';

    this.selectedValue.setValue({ code: selectedCode, name: selectedDesc });
  }

  clear(): void {
    this.selectedLevel1 = undefined;
    this.selectedLevel2 = undefined;
    this.selectedLevel3 = undefined;
    this.selectedValue.reset();
  }

  // 🔹 helper: recursively search for default code
  private setDefaultSelection(defaultCode: string): void {
    for (const level1 of this.itemGroups) {
      if (level1.code === defaultCode) {
        this.selectedLevel1 = level1;
        this.updateSelectedCode();
        return;
      }
      for (const level2 of level1.childItemGroups ?? []) {
        if (level2.code === defaultCode) {
          this.selectedLevel1 = level1;
          this.selectedLevel2 = level2;
          this.updateSelectedCode();
          return;
        }
        for (const level3 of level2.childItemGroups ?? []) {
          if (level3.code === defaultCode) {
            this.selectedLevel1 = level1;
            this.selectedLevel2 = level2;
            this.selectedLevel3 = level3;
            this.updateSelectedCode();
            return;
          }
        }
      }
    }
  }
}
