<script lang="ts">
  interface Props {
    label?: string
    size?: "sm" | "md" | "lg"
  }
  let { label = "Laster inn..", size = "sm" }: Props = $props()
</script>

<div class="spinner-wrapper">
  <svg
    aria-label={label}
    class="ds-spinner"
    role="img"
    viewBox="0 0 50 50"
    data-size={size}
  >
    <circle
        class="ds-spinner__background"
        cx="25"
        cy="25"
        r="20"
        fill="none"
        stroke-width="5"
    ></circle>

    <circle
        class="ds-spinner__circle"
        cx="25"
        cy="25"
        r="20"
        fill="none"
        stroke-width="5"
    ></circle>
  </svg>
  <p class="ds-paragraph">{label}</p>
</div>

<style>
  .spinner-wrapper {
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }

  .spinner-wrapper p {
    margin: 0;
  }
</style>
